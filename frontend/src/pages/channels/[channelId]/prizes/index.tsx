import { Heading } from "@chakra-ui/react";
import { useBreadcrumbs } from "components/Breadcrumbs/useBreadcrumbs";
import PrizeListOverview from "components/Prizes/PrizeListOverview";
import { withAuth } from "hocs/withAuth";
import { NextPage } from "next";
import { useRouter } from "next/router";
import React, { useMemo } from "react";

const IndexPage: NextPage = () => {
  const { query } = useRouter();
  const channelId = useMemo(() => {
    if (!query.channelId) return;
    const channelId = parseInt(query.channelId as string);
    return channelId;
  }, [query]);

  useBreadcrumbs([
    {
      name: "home",
      path: "/"
    },
    {
      name: "channel " + channelId,
      path: "/channels/[channelId]/rounds",
      asPath: `/channels/${channelId}/rounds`
    },
    {
      name: "prizes",
      path: "/channels/[channelId]/prizes",
      asPath: `/channels/${channelId}/prizes`
    }
  ]);

  return (
    <>
      {/* <PrizeSignalRContext.Provider value={hub}></PrizeSignalRContext.Provider> */}
      <Heading textAlign="center">Prizes</Heading>

      <PrizeListOverview />
    </>
  );
};

export default withAuth(IndexPage);