import { Box } from "@chakra-ui/react";
import { useBreadcrumbs } from "components/Breadcrumbs/useBreadcrumbs";
import { AuthContext } from "contexts/AuthContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { NextPage } from "next";
import { useRouter } from "next/dist/client/router";
import React, { useContext, useState } from "react";
import { genChannelClient } from "services/backend/apiClients";
import { RoundSnipDto } from "services/backend/nswagts";
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

  const [rounds, setRounds] = useState<RoundSnipDto[]>([]);

  useEffectAsync(async () => {
    if (activeUser && router.query.channelId) {
      const channelId = parseInt(router.query.channelId as string);

      const client = await genChannelClient();
      const result = await client.getRounds(channelId);

      setRounds(result);
    }
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
          backgroundColor={round.active ? "green.700" : "blue.700"}>
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
