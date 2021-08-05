import "ts-array-ext/findAndReplace";

import { Heading, VStack } from "@chakra-ui/react";
import { ChosenChannelContext } from "components/Common/AppContainer/ChosenChannelContext";
import { DividerWithText } from "components/Common/DividerWIthText";
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

  const [prizes, setPrizes] = useState<PrizeIdDTO[]>([]);

  const { hub, Provider } = useHubProvider("prize", {
    autoCloseOnUnmount: true
  });

  const { genClient } = useNSwagClient(PrizesClient);

  useEffect(() => {
    hub?.onConnect("newPrize", newPrize => {
      console.log("newPrize", newPrize);
      if (newPrize.channelSettingsId === chosenChannel.id) setPrizes(p => [...p, newPrize]);
    });

    hub?.onConnect("updatedPrize", updatedPrize => {
      console.log("updatedPrize", updatedPrize);
      setPrizes(p => {
        p.findAndReplace(x => x.id === updatedPrize.id, updatedPrize, true);
        return [...p];
      });
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
      <DividerWithText py={4}>
        <Heading size="sm">Milestones</Heading>
      </DividerWithText>
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

      <DividerWithText py={4}>
        <Heading size="sm">Repeatable</Heading>
      </DividerWithText>
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
      <DividerWithText py={4}>
        <Heading size="sm">Other</Heading>
      </DividerWithText>
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
