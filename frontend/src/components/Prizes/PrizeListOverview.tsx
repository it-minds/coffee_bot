import { Heading, VStack } from "@chakra-ui/react";
import { ChosenChannelContext } from "components/Common/AppContainer/ChosenChannelContext";
import { useHubProvider } from "contexts/SignalRContext/useHubProvider";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import React, { useContext, useState } from "react";
import { useEffect } from "react";
import { FC } from "react";
import { PrizeIdDTO, PrizesClient } from "services/backend/nswagts";

import AvailablePrize from "./AvailablePrize";
import BoxCover from "./BoxCover";
import PurchaseButton from "./PurchaseButton";

const PrizeListOverview: FC = () => {
  const { chosenChannel } = useContext(ChosenChannelContext);

  const { hub, Provider } = useHubProvider("prize", true);

  const [prizes, setPrizes] = useState<PrizeIdDTO[]>([]);

  const { genClient } = useNSwagClient(PrizesClient);

  useEffect(() => {
    hub?.onConnect("newPrize", newPrize => {
      if (newPrize.channelSettingsId === chosenChannel.id) setPrizes(p => [...p, newPrize]);
    });
  }, [hub]);

  useEffectAsync(async () => {
    if (!chosenChannel.id) return;

    const client = await genClient();

    const allPrizes: PrizeIdDTO[] = await client.getChannelPrizes(chosenChannel.id).catch(() => []);

    setPrizes(allPrizes);
  }, [chosenChannel.id]);

  return (
    <Provider value={hub}>
      <Heading size="ms">Milestones</Heading>
      <VStack spacing={4}>
        {prizes
          .filter(x => x.isMilestone)
          .map(p => (
            <BoxCover key={"avail-other-" + p.id}>
              <AvailablePrize prize={p}>
                <PurchaseButton prize={p} isPurchasable={false} />
              </AvailablePrize>
            </BoxCover>
          ))}
      </VStack>

      <Heading size="ms">Repeatable</Heading>
      <VStack spacing={4}>
        {prizes
          .filter(x => x.isRepeatable)
          .map(p => (
            <BoxCover key={"avail-other-" + p.id}>
              <AvailablePrize prize={p}>
                <PurchaseButton prize={p} isPurchasable={false} />
              </AvailablePrize>
            </BoxCover>
          ))}
      </VStack>

      <Heading size="ms">Other</Heading>
      <VStack spacing={4}>
        {prizes
          .filter(x => !x.isMilestone && !x.isRepeatable)
          .map(p => (
            <BoxCover key={"avail-other-" + p.id}>
              <AvailablePrize prize={p}>
                <PurchaseButton prize={p} isPurchasable={false} />
              </AvailablePrize>
            </BoxCover>
          ))}
      </VStack>
    </Provider>
  );
};

export default PrizeListOverview;
