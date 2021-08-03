import "ts-array-ext/groupBy";

import { Box, Center, Heading, HStack, Spacer, VStack } from "@chakra-ui/react";
import { ChosenChannelContext } from "components/Common/AppContainer/ChosenChannelContext";
import OurSpinner from "components/Common/OurSpinner";
import AvailablePrize from "components/Prizes/AvailablePrize";
import BoxCover from "components/Prizes/BoxCover";
import PurchaseButton from "components/Prizes/PurchaseButton";
import { useHubProvider } from "contexts/SignalRContext/useHubProvider";
import { withAuth } from "hocs/withAuth";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import { NextPage } from "next";
import React, { useCallback, useContext, useState } from "react";
import { PrizesClient, UserPrizesDTO } from "services/backend/nswagts";

const IndexPage: NextPage = () => {
  const { chosenChannel } = useContext(ChosenChannelContext);

  const { hub, Provider } = useHubProvider("prize", true);

  const [loading, setLoading] = useState(false);
  const [prizes, setPrizes] = useState<UserPrizesDTO>(null);

  const { genClient } = useNSwagClient(PrizesClient);

  useEffectAsync(async () => {
    if (chosenChannel.id === null) return;
    const client = await genClient();
    const allPrizes: UserPrizesDTO = await client.getMyPrizes(chosenChannel.id).catch(() => null);
    setPrizes(allPrizes);
  }, [chosenChannel.id]);

  const refetch = useCallback(async () => {
    setLoading(true);
    const client = await genClient();
    const allPrizes: UserPrizesDTO = await client.getMyPrizes(chosenChannel.id).catch(() => null);
    setPrizes(allPrizes);
    setLoading(false);
  }, [chosenChannel.id]);

  return (
    <>
      <Provider value={hub}></Provider>
      <Heading textAlign="center">My Prizes</Heading>

      {loading && (
        <Box position="fixed" w={"6xl"} minH={24} zIndex={2} pt={2}>
          <Center>
            <OurSpinner />
          </Center>
        </Box>
      )}

      <Box pointerEvents={loading ? "none" : "inherit"} opacity={loading ? 0.4 : 1}>
        <HStack>
          <p>Rank: {prizes?.points}</p>
          <Spacer />
          <p>
            Points available: {prizes?.pointsRemaining}
            <i>p</i>
          </p>
        </HStack>

        <Heading size="md" mt={2} mb={2}>
          Claimed Prizes
        </Heading>
        <VStack spacing={2}>
          {Object.entries(
            prizes?.prizesClaimed.groupBy(
              x => x.prize.id,
              a => a
            ) ?? {}
          ).map(([prizeId, prizes]) => {
            const p = prizes[0];
            let title = (p.prize.isMilestone ? "Milestone: " : "") + p.prize.title;
            if (p.prize.isRepeatable) title += ` (repeated ${prizes.length} times)`;
            return (
              <BoxCover key={"claimed-milestone-" + prizeId}>
                <AvailablePrize
                  isClaimed
                  prize={{
                    title: title
                  }}>
                  Have bought
                </AvailablePrize>
              </BoxCover>
            );
          })}
        </VStack>
        <Heading size="md" mt={2} mb={2}>
          Available for purchase
        </Heading>
        {prizes?.prizesAvailable.filter(x => x.isMilestone).length > 0 && (
          <Heading size="sm">Milestones</Heading>
        )}
        <VStack spacing={4}>
          {prizes?.prizesAvailable
            .filter(x => x.isMilestone)
            .map(p => (
              <BoxCover key={"avail-milestone-" + p.id}>
                <AvailablePrize prize={p}>
                  <PurchaseButton prize={p} addCallback={refetch} />
                </AvailablePrize>
              </BoxCover>
            ))}
        </VStack>
        {prizes?.prizesAvailable.filter(x => x.isRepeatable).length > 0 && (
          <Heading size="sm">Repeatable</Heading>
        )}
        <VStack spacing={4}>
          {prizes?.prizesAvailable
            .filter(x => x.isRepeatable)
            .map(p => (
              <BoxCover key={"avail-repeatable-" + p.id}>
                <AvailablePrize prize={p}>
                  <PurchaseButton prize={p} addCallback={refetch} />
                </AvailablePrize>
              </BoxCover>
            ))}
        </VStack>
        {prizes?.prizesAvailable.filter(x => !x.isMilestone && !x.isRepeatable).length > 0 && (
          <Heading size="sm">One-Time</Heading>
        )}
        <VStack spacing={4}>
          {prizes?.prizesAvailable
            .filter(x => !x.isMilestone && !x.isRepeatable)
            .map(p => (
              <BoxCover key={"avail-other-" + p.id}>
                <AvailablePrize prize={p}>
                  <PurchaseButton prize={p} addCallback={refetch} />
                </AvailablePrize>
              </BoxCover>
            ))}
        </VStack>
      </Box>
    </>
  );
};

export default withAuth(IndexPage);
