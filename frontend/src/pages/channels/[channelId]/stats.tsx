import { Heading, Table, Tbody, Td, Th, Thead, Tr } from "@chakra-ui/react";
import { useBreadcrumbs } from "components/Breadcrumbs/useBreadcrumbs";
import QuerySortBtn from "components/Common/QuerySortBtn";
import { useEffectAsync } from "hooks/useEffectAsync";
import { NextPage } from "next";
import { useRouter } from "next/router";
import React, { useState } from "react";
import { useCallback } from "react";
import { genStatsClient } from "services/backend/apiClients";
import { StatsDto } from "services/backend/nswagts";

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

  const [stats, setStats] = useState<StatsDto[]>([]);

  const [sortCb, setSortCB] = useState<(a: StatsDto, b: StatsDto) => number>(() => defaultSort);

  useEffectAsync(async () => {
    if (query.channelId) {
      const channelId = parseInt(query.channelId as string);

      const client = await genStatsClient();
      const allImages: StatsDto[] = await client.getMemberStats(channelId).catch(() => []);

      setStats(allImages);
    }
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
            <Th>User Id</Th>
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
              <Td>{stat.slackMemberId}</Td>
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
