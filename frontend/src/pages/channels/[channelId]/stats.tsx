import { Heading, Table, Tbody, Td, Th, Thead, Tr, useBreakpointValue } from "@chakra-ui/react";
import { useBreadcrumbs } from "components/Breadcrumbs/useBreadcrumbs";
import QuerySortBtn from "components/Common/QuerySortBtn";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import { NextPage } from "next";
import { useRouter } from "next/router";
import React, { useState } from "react";
import { useCallback } from "react";
import { IStatsClient, StatsClient, StatsDto } from "services/backend/nswagts";

const defaultSort = (a: StatsDto, b: StatsDto) => a.slackMemberId.localeCompare(b.slackMemberId);

const formatter = new Intl.NumberFormat("en-US", {
  maximumFractionDigits: 2,
  minimumFractionDigits: 2
});

const IndexPage: NextPage = () => {
  // const { activeUser } = useContext(AuthContext);
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
      name: "stats",
      path: "/channels/[channelId]/stats",
      asPath: `/channels/${query.channelId}/stats`
    }
  ]);

  const showId = useBreakpointValue({
    base: false,
    md: true
  });

  const [stats, setStats] = useState<StatsDto[]>([]);

  const [sortCb, setSortCB] = useState<(a: StatsDto, b: StatsDto) => number>(() => defaultSort);

  const { genClient } = useNSwagClient<IStatsClient>(StatsClient);

  useEffectAsync(async () => {
    if (!query.channelId) return;
    const channelId = parseInt(query.channelId as string);

    const client = await genClient();
    const allImages: StatsDto[] = await client.getMemberStats(channelId).catch(() => []);

    setStats(allImages);
  }, [query]);

  const sort = useCallback((key: keyof StatsDto, direction: "ASC" | "DESC") => {
    if (direction === "ASC") {
      setSortCB(() => (a: StatsDto, b: StatsDto) => (a[key] as number) - (b[key] as number));
    }
    if (direction === "DESC") {
      setSortCB(() => (a: StatsDto, b: StatsDto) => (b[key] as number) - (a[key] as number));
    }
    if (direction === null) {
      setSortCB(() => defaultSort);
    }
  }, []);

  return (
    <>
      <Heading textAlign="center">Statistics</Heading>

      <Table variant="striped" colorScheme="gray" size="sm">
        <Thead>
          <Tr>
            {showId && <Th>User Id</Th>}
            <Th>Name</Th>
            <Th isNumeric>
              Meetup %
              <QuerySortBtn queryKey="meepupPercent" sortCb={sort} />
            </Th>
            <Th isNumeric>
              Photo %
              <QuerySortBtn queryKey="photoPercent" sortCb={sort} />
            </Th>
          </Tr>
        </Thead>
        <Tbody>
          {stats.sort(sortCb).map(stat => (
            <Tr key={stat.slackMemberId}>
              {showId && <Td>{stat.slackMemberId}</Td>}
              <Td>{stat.slackMemberName}</Td>
              <Td isNumeric>{formatter.format(stat.meepupPercent)}</Td>
              <Td isNumeric>{formatter.format(stat.photoPercent)}</Td>
            </Tr>
          ))}
        </Tbody>
      </Table>
    </>
  );
};

export default IndexPage;
