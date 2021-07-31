import { Heading } from "@chakra-ui/react";
import NewPrizeModal from "components/Prizes/NewPrizeModal";
import PrizeListOverview from "components/Prizes/PrizeListOverview";
import { AuthContext } from "contexts/AuthContext";
import { withAuth } from "hocs/withAuth";
import { NextPage } from "next";
import { useRouter } from "next/router";
import React, { useContext, useEffect, useMemo } from "react";

const IndexPage: NextPage = () => {
  const { query, replace } = useRouter();
  const channelId = useMemo(() => {
    if (!query.channelId) return;
    const channelId = parseInt(query.channelId as string);
    return channelId;
  }, [query]);
  const { activeUser } = useContext(AuthContext);

  useEffect(() => {
    if (activeUser && channelId && !activeUser.channelsToAdmin.includes(channelId)) {
      replace("/");
    }
  }, [activeUser, channelId]);

  return (
    <>
      {/* <PrizeSignalRContext.Provider value={hub}></PrizeSignalRContext.Provider> */}
      <Heading textAlign="center">Prizes</Heading>

      <NewPrizeModal />

      <PrizeListOverview />
    </>
  );
};

export default withAuth(IndexPage);
