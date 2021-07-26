import { Heading, Table, Tbody, Td, Th, Thead, Tr } from "@chakra-ui/react";
import { useBreadcrumbs } from "components/Breadcrumbs/useBreadcrumbs";
import QuerySortBtn from "components/Common/QuerySortBtn";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNSwagClient";
import { NextPage } from "next";
import { useRouter } from "next/router";
import React, { useState } from "react";
import { useReducer } from "react";
import { useCallback } from "react";
import ListReducer, { ListReducerActionType } from "react-list-reducer";
import { StatsClient, StatsDto } from "services/backend/nswagts";

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

  const [sortCb, setSortCB] = useReducer(
    ListReducer<{ id: string; compareFn: (a: StatsDto, b: StatsDto) => number }>("id"),
    []
  );

  const { genClient } = useNSwagClient(StatsClient);

  useEffectAsync(async () => {
    if (!query.channelId) return;
    const channelId = parseInt(query.channelId as string);

    const client = await genClient();
    const allImages: StatsDto[] = await client.getMemberStats(channelId).catch(() => []);

    setStats(allImages);
  }, [query]);

  const sort = useCallback((_group: string, key: keyof StatsDto, direction: "ASC" | "DESC") => {
    if (direction === "ASC") {
      setSortCB({
        type: ListReducerActionType.AddOrUpdate,
        data: {
          id: key,
          compareFn: (a: StatsDto, b: StatsDto) => (a[key] > b[key] ? 1 : a[key] < b[key] ? -1 : 0)
        }
      });
    }
    if (direction === "DESC") {
      setSortCB({
        type: ListReducerActionType.AddOrUpdate,
        data: {
          id: key,
          compareFn: (a: StatsDto, b: StatsDto) => (a[key] < b[key] ? 1 : a[key] > b[key] ? -1 : 0)
        }
      });
    }
    if (direction === null) {
      setSortCB({
        type: ListReducerActionType.Remove,
        data: key
      });
    }
  }, []);

  return (
    <>
      <Heading textAlign="center">Statistics</Heading>

      <Table variant="striped" colorScheme="gray" size="sm">
        <Thead>
          <Tr>
            <Th>Name</Th>
            <Th isNumeric>
              Points
              <QuerySortBtn
                queryGroup="a"
                queryKey="points"
                sortCb={sort}
                defaultDirection="DESC"
              />
            </Th>
            <Th isNumeric>
              Meetup %
              <QuerySortBtn queryGroup="a" queryKey="meepupPercent" sortCb={sort} />
            </Th>
            <Th isNumeric>
              Photo %
              <QuerySortBtn queryGroup="a" queryKey="photoPercent" sortCb={sort} />
            </Th>
          </Tr>
        </Thead>
        <Tbody>
          {stats
            .sort((a, b) => {
              let result = 0;
              sortCb.some(x => {
                const res = x.compareFn(a, b);
                if (res != 0) {
                  result = res;
                  return true;
                }
              });
              if (result === 0) {
                return defaultSort(a, b);
              }
              return result;
            })
            .map(stat => (
              <Tr key={stat.slackMemberId}>
                <Td>{stat.slackMemberName || stat.slackMemberId}</Td>
                <Td isNumeric>{stat.points}</Td>
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
