import { Heading, VStack } from "@chakra-ui/react";
import { useBreadcrumbs } from "components/Breadcrumbs/useBreadcrumbs";
import AvailablePrize from "components/Prizes/AvailablePrize";
import BoxCover from "components/Prizes/BoxCover";
import { AuthContext } from "contexts/AuthContext";
import { MyHub, PrizeSignalRContext } from "contexts/SignalRContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import { NextPage } from "next";
import { useRouter } from "next/router";
import React, { useContext, useEffect, useMemo, useState } from "react";
import { IPrizeIdDTO, PrizesClient } from "services/backend/nswagts";

const IndexPage: NextPage = () => {
  const { query, replace } = useRouter();
  const channelId = useMemo(() => {
    if (!query.channelId) return;
    const channelId = parseInt(query.channelId as string);
    return channelId;
  }, [query]);
  const { activeUser } = useContext(AuthContext);

  useEffect(() => {
    if (activeUser && channelId && !activeUser.channelsToAdmin.includes(channelId)) {
      replace("/");
    }
  }, [activeUser, channelId]);

  useBreadcrumbs([
    {
      name: "home",
      path: "/"
    },
    {
      name: "channel " + channelId,
      path: "/channels/[channelId]/rounds",
      asPath: `/channels/${channelId}/rounds`
    },
    {
      name: "prizes",
      path: "/channels/[channelId]/prizes",
      asPath: `/channels/${channelId}/prizes`
    }
  ]);

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
      <PrizeSignalRContext.Provider value={hub}></PrizeSignalRContext.Provider>
      <Heading textAlign="center">Prizes</Heading>

      <Heading size="ms">Milestones</Heading>
      <VStack spacing={4}>
        {prizes
          .filter(x => x.isMilestone)
          .map(p => (
            <BoxCover key={"avail-other-" + p.id}>
              <AvailablePrize prize={p} isPurchasable={false} />
            </BoxCover>
          ))}
      </VStack>

      <Heading size="ms">Repeatable</Heading>
      <VStack spacing={4}>
        {prizes
          .filter(x => x.isRepeatable)
          .map(p => (
            <BoxCover key={"avail-other-" + p.id}>
              <AvailablePrize prize={p} isPurchasable={false} />
            </BoxCover>
          ))}
      </VStack>

      <Heading size="ms">Other</Heading>
      <VStack spacing={4}>
        {prizes
          .filter(x => !x.isMilestone && !x.isRepeatable)
          .map(p => (
            <BoxCover key={"avail-other-" + p.id}>
              <AvailablePrize prize={p} isPurchasable={false} />
            </BoxCover>
          ))}
      </VStack>
    </>
  );
};

export default IndexPage;
