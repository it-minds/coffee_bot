import { Heading, VStack } from "@chakra-ui/react";
import { MyHub } from "contexts/SignalRContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import { useRouter } from "next/router";
import React, { useMemo, useState } from "react";
import { FC } from "react";
import { IPrizeIdDTO, PrizesClient } from "services/backend/nswagts";

import AvailablePrize from "./AvailablePrize";
import BoxCover from "./BoxCover";
import PurchaseButton from "./PurchaseButton";

const PrizeListOverview: FC = () => {
  const { query } = useRouter();
  const channelId = useMemo(() => {
    if (!query.channelId) return;
    const channelId = parseInt(query.channelId as string);
    return channelId;
  }, [query]);

  const [hub, setHub] = useState<MyHub<"prize">>();
  const [prizes, setPrizes] = useState<IPrizeIdDTO[]>([]);

  const { genClient } = useNSwagClient(PrizesClient);

  useEffectAsync(async () => {
    if (!channelId) return;

    const hub = await MyHub.startConnection("prize");
    const client = await genClient();

    client.addSignalRConnectionId(hub.getConnection().connectionId);

    const allPrizes: IPrizeIdDTO[] = await client.getChannelPrizes(channelId).catch(() => []);
    setPrizes(allPrizes);

    setHub(hub);
    hub.onConnect("NewPrize", newPrize => {
      if (newPrize.channelSettingsId === channelId) setPrizes(p => [...p, newPrize]);
    });
  }, [channelId]);

  return (
    <>
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
    </>
  );
};

export default PrizeListOverview;
