import { Box, useColorModeValue } from "@chakra-ui/react";
import { useBreadcrumbs } from "components/Breadcrumbs/useBreadcrumbs";
import { AuthContext } from "contexts/AuthContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import { NextPage } from "next";
import { useRouter } from "next/dist/client/router";
import React, { useContext, useState } from "react";
import { ChannelClient, RoundSnipDto } from "services/backend/nswagts";
import { dateTimeFormatter } from "utils/formatters/dateTimeFormatter";
import { percentFormatter } from "utils/formatters/percentFormatter";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);
  const router = useRouter();

  useBreadcrumbs([
    {
      name: "home",
      path: "/"
    },
    {
      name: "channel " + router.query.channelId,
      path: "/channels/[channelId]/rounds",
      asPath: `/channels/${router.query.channelId}/rounds`
    },
    {
      name: "rounds",
      path: "/channels/[channelId]/rounds",
      asPath: `/channels/${router.query.channelId}/rounds`
    }
  ]);

  const activeColor = useColorModeValue("green.200", "green.600");
  const normalColor = useColorModeValue("blue.100", "blue.700");

  const [rounds, setRounds] = useState<RoundSnipDto[]>([]);

  const { genClient } = useNSwagClient(ChannelClient);

  useEffectAsync(async () => {
    if (!activeUser || !router.query.channelId) return;
    const channelId = parseInt(router.query.channelId as string);

    const client = await genClient();
    const result = await client.getRounds(channelId);

    setRounds(result);
  }, [activeUser, router.query]);

  return (
    <>
      {rounds.map(round => (
        <Box
          key={round.id}
          onClick={() =>
            router.push(
              "/channels/[channelId]/rounds/[roundId]",
              `/channels/${router.query.channelId}/rounds/${round.id}`
            )
          }
          m={2}
          p={2}
          cursor="pointer"
          backgroundColor={round.active ? activeColor : normalColor}>
          Round: {dateTimeFormatter.format(round.startDate)} -{" "}
          {dateTimeFormatter.format(round.endDate)}
          <br />
          Meetup: {percentFormatter.format(round.meetupPercentage)}
          <br />
          Photo: {percentFormatter.format(round.photoPercentage)}
        </Box>
      ))}
    </>
  );
};

export default IndexPage;
