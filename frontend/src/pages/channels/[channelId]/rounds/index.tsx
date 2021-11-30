import { HStack, Input, Text, useColorModeValue, useToken } from "@chakra-ui/react";
import { ChartData, ChartOptions } from "chart.js";
import { withAuth } from "hocs/withAuth";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import { NextPage } from "next";
import { useRouter } from "next/dist/client/router";
import React, { useEffect, useMemo, useState } from "react";
import { Bar } from "react-chartjs-2";
import { ChannelClient, RoundSnipDto } from "services/backend/nswagts";
import { dateTimeFormatter } from "utils/formatters/dateTimeFormatter";

const IndexPage: NextPage = () => {
  const router = useRouter();
  const channelId = useMemo(() => parseInt(router.query.channelId as string), [router.query]);

  const [fromDate, setFromDate] = useState<string>(
    () =>
      (router.query.from as string) ??
      new Date(new Date().valueOf() - 5 * 7 * 24 * 3600 * 1000).toISOString().substring(0, 10)
  );
  const [toDate, setToDate] = useState<string>(new Date().toISOString().substring(0, 10));
  const [rounds, setRounds] = useState<RoundSnipDto[]>([]);

  const { genClient } = useNSwagClient(ChannelClient);

  useEffectAsync(async () => {
    const client = await genClient();
    const result = await client.getRoundsInRange(channelId, new Date(fromDate), new Date(toDate));

    setRounds(result);
  }, [fromDate, toDate, channelId]);

  useEffect(() => {
    const query = `?from=${fromDate}&to=${toDate}`;
    const safeAsPath = router.asPath.split("?")[0];

    router.replace(router.pathname + query, safeAsPath + query, {
      shallow: true
    });
  }, [fromDate, toDate, router.pathname]);

  const [bar1Color, bar2Color] = useToken("colors", [
    useColorModeValue("blue.300", "blue.700"),
    useColorModeValue("pink.400", "pink.700")
  ]);

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
          backgroundColor: bar1Color
        },
        {
          label: "Photo percentage",
          data: rounds.map(round => (round.photoPercentage > 0 ? round.photoPercentage : 1)),
          backgroundColor: bar2Color
        }
      ]
    });
    setOptions({
      onHover: (event, elements) => {
        //@ts-expect-error it says style property does not exist, but this works as expected
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
  }, [rounds, bar1Color, bar2Color]);

  return (
    <>
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
      </HStack>
      <Bar
        data={data}
        options={options}
        onMouseLeave={event => {
          //@ts-expect-error it says style property does not exist, but this works as expected
          event.target.style.cursor = "default";
        }}
      />
    </>
  );
};

export default withAuth(IndexPage);
