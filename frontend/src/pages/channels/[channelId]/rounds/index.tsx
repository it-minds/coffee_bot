import { useColorModeValue } from "@chakra-ui/react";
import { ChartData, ChartOptions } from "chart.js";
import { AuthContext } from "contexts/AuthContext";
import { withAuth } from "hocs/withAuth";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import { NextPage } from "next";
import { useRouter } from "next/dist/client/router";
import React, { useContext, useEffect, useState } from "react";
import { Bar } from "react-chartjs-2";
import { ChannelClient, RoundSnipDto } from "services/backend/nswagts";
import { dateTimeFormatter } from "utils/formatters/dateTimeFormatter";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);
  const router = useRouter();

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

  const [data, setData] = useState<ChartData>();
  const [options, setOptions] = useState<ChartOptions>();
  useEffect(() => {
    setData({
      labels: rounds.map(
        round =>
          dateTimeFormatter.format(new Date(round.startDate)) +
          " - " +
          dateTimeFormatter.format(new Date(round.endDate))
      ),
      datasets: [
        {
          label: "Meetup percentage",
          data: rounds.map(round => (round.meetupPercentage > 0 ? round.meetupPercentage : 1)),
          backgroundColor: "rgb(255, 0, 0)"
        },
        {
          label: "Photo percentage",
          data: rounds.map(round => (round.photoPercentage > 0 ? round.photoPercentage : 1)),
          backgroundColor: "rgb(0, 255, 0)"
        }
      ]
    });
    setOptions({
      onHover: (event, elements) => {
        event.native.target.style.cursor = elements.length > 0 ? "pointer" : "default";
      },
      onClick: (event, elements) => {
        if (elements.length == 0) return;
        router.push(
          "/channels/[channelId]/rounds/[roundId]",
          `/channels/${router.query.channelId}/rounds/${rounds[elements[0].index].id}`
        );
      },
      scales: {
        y: {
          min: 0,
          max: 100
        }
      }
    });
  }, [rounds]);

  return (
    <>
      <Bar
        data={data}
        options={options}
        onMouseLeave={event => {
          event.target.style.cursor = "default";
        }}
      />
    </>
  );
};

export default withAuth(IndexPage);
