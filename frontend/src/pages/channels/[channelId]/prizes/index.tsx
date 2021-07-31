import { Heading } from "@chakra-ui/react";
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

  return (
    <>
      {/* <PrizeSignalRContext.Provider value={hub}></PrizeSignalRContext.Provider> */}
      <Heading textAlign="center">Prizes</Heading>

      <PrizeListOverview />
    </>
  );
};

export default withAuth(IndexPage);
