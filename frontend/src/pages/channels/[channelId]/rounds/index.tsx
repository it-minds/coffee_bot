import { Button, HStack, Input, Text } from "@chakra-ui/react";
import { ChartData, ChartOptions } from "chart.js";
import { AuthContext } from "contexts/AuthContext";
import { withAuth } from "hocs/withAuth";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import { NextPage } from "next";
import { useRouter } from "next/dist/client/router";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { Bar } from "react-chartjs-2";
import { ChannelClient, RoundSnipDto } from "services/backend/nswagts";
import { dateTimeFormatter } from "utils/formatters/dateTimeFormatter";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);
  const router = useRouter();

  const [channelId, setChannelId] = useState(0);
  const [fromDate, setFromDate] = useState<string>(
    // 5 weeks ago as default value
    new Date(new Date().valueOf() - 5 * 7 * 24 * 3600 * 1000).toISOString().substring(0, 10)
  );
  const [toDate, setToDate] = useState<string>(new Date().toISOString().substring(0, 10));
  const [rounds, setRounds] = useState<RoundSnipDto[]>([]);

  const { genClient } = useNSwagClient(ChannelClient);

  const fetchRounds = useCallback(async () => {
    const client = await genClient();
    const result = await client.getRoundsInRange(channelId, new Date(fromDate), new Date(toDate));

    setRounds(result);
  }, [fromDate, toDate, setRounds, channelId]);

  useEffectAsync(async () => {
    if (!activeUser || !router.query.channelId) return;
    setChannelId(parseInt(router.query.channelId as string));
  }, [activeUser, router.query]);

  useEffect(() => {
    fetchRounds();
  }, [channelId]);

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
        //it says style property does not exist, but this works as if it does
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
      <form
        onSubmit={e => {
          e.preventDefault();
          fetchRounds();
        }}>
        <HStack>
          <Text>Date interval: From: </Text>
          <Input
            type="date"
            w="xsm"
            value={fromDate}
            onChange={event => setFromDate(event.target.value)}
          />
          <Text> To: </Text>
          <Input
            type="date"
            w="xsm"
            value={toDate}
            onChange={event => setToDate(event.target.value)}
          />
          <Input type="submit" as={Button} w="sm">
            Get rounds from selected interval
          </Input>
        </HStack>
      </form>
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
