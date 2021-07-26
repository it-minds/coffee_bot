import { Box, Center, Heading, VStack } from "@chakra-ui/react";
import { useBreadcrumbs } from "components/Breadcrumbs/useBreadcrumbs";
import OurSpinner from "components/Common/OurSpinner";
import AvailablePrize from "components/Prizes/AvailablePrize";
import BoxCover from "components/Prizes/BoxCover";
import { MyHub, PrizeSignalRContext } from "contexts/SignalRContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import { NextPage } from "next";
import { useRouter } from "next/router";
import React, { useCallback, useState } from "react";
import { useMemo } from "react";
import { IUserPrizesDTO, PrizesClient } from "services/backend/nswagts";

const IndexPage: NextPage = () => {
  const { query } = useRouter();

  useBreadcrumbs([
    {
      name: "home",
      path: "/"
    },
    {
      name: "channel " + query.channelId,
      path: "/channels/[channelId]/rounds",
      asPath: `/channels/${query.channelId}/rounds`
    },
    {
      name: "prizes",
      path: "/channels/[channelId]",
      asPath: `/channels/${query.channelId}`
    },
    {
      name: "My Prizes",
      path: "/channels/[channelId]/prizes/mine",
      asPath: `/channels/${query.channelId}/prizes/mine`
    }
  ]);

  const [hub, setHub] = useState<MyHub<"prize">>();
  const [loading, setLoading] = useState(false);
  const [prizes, setPrizes] = useState<IUserPrizesDTO>(null);

  const { genClient } = useNSwagClient(PrizesClient);

  const channelId = useMemo(() => {
    if (!query.channelId) return;
    const channelId = parseInt(query.channelId as string);
    return channelId;
  }, [query]);

  useEffectAsync(async () => {
    if (channelId === null) return;
    const client = await genClient();
    const allPrizes: IUserPrizesDTO = await client.getMyPrizes(channelId).catch(() => null);
    setPrizes(allPrizes);

    const hub = await MyHub.startConnection("prize");
    client.addSignalRConnectionId(hub.getConnection().connectionId);
    setHub(hub);
    // hub.onConnect("NewPrize", newPrize => {
    //   if (newPrize.channelSettingsId === channelId) setPrizes(p => [...p, newPrize]);
    // });
  }, [channelId]);

  const refetch = useCallback(async () => {
    setLoading(true);
    const client = await genClient();
    const allPrizes: IUserPrizesDTO = await client.getMyPrizes(channelId).catch(() => null);
    setPrizes(allPrizes);
    setLoading(false);
  }, [channelId]);

  return (
    <>
      <PrizeSignalRContext.Provider value={hub}></PrizeSignalRContext.Provider>
      <Heading textAlign="center">My Prizes</Heading>

      {loading && (
        <Box position="fixed" w={"6xl"} minH={24} zIndex={2} pt={2}>
          <Center>
            <OurSpinner />
          </Center>
        </Box>
      )}

      <Box pointerEvents={loading ? "none" : "inherit"} opacity={loading ? 0.4 : 1}>
        <p>Rank: {prizes?.points}</p>
        <p>
          Points: {prizes?.pointsRemaining}
          <i>p</i>
        </p>
        <Heading size="md">Claimed Prizes</Heading>
        <VStack spacing={2}>
          {prizes?.prizesClaimed.map(p => (
            <BoxCover key={"claimed-milestone-" + p.id}>
              <p>
                Date: {p.dateClaimed.toLocaleDateString()}
                <br />
                Title: {p.prizeTitle}
                <br />
                Cost: {p.pointCost}
              </p>
            </BoxCover>
          ))}
        </VStack>
        <Heading size="md">Available for purchase</Heading>
        <Heading size="sm">Milestones</Heading>
        <VStack spacing={4}>
          {prizes?.prizesAvailable
            .filter(x => x.isMilestone)
            .map(p => (
              <BoxCover key={"avail-milestone-" + p.id}>
                <AvailablePrize prize={p} addCallback={refetch} />
              </BoxCover>
            ))}
        </VStack>
        <Heading size="sm">Repeatable</Heading>
        <VStack spacing={4}>
          {prizes?.prizesAvailable
            .filter(x => x.isRepeatable)
            .map(p => (
              <BoxCover key={"avail-repeatable-" + p.id}>
                <AvailablePrize prize={p} addCallback={refetch} />
              </BoxCover>
            ))}
        </VStack>
        <Heading size="sm">Other</Heading>
        <VStack spacing={4}>
          {prizes?.prizesAvailable
            .filter(x => !x.isMilestone && !x.isRepeatable)
            .map(p => (
              <BoxCover key={"avail-other-" + p.id}>
                <AvailablePrize prize={p} addCallback={refetch} />
              </BoxCover>
            ))}
        </VStack>
      </Box>
    </>
  );
};

export default IndexPage;
