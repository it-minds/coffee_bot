import { Heading } from "@chakra-ui/react";
import { ChosenChannelContext } from "components/Common/AppContainer/ChosenChannelContext";
import NewPrizeModal from "components/Prizes/NewPrizeModal";
import PrizeListOverview from "components/Prizes/PrizeListOverview";
import { AuthContext } from "contexts/AuthContext";
import { withAuth } from "hocs/withAuth";
import { NextPage } from "next";
import { useRouter } from "next/router";
import React, { useContext, useEffect } from "react";

const IndexPage: NextPage = () => {
  const { replace } = useRouter();
  const { chosenChannel } = useContext(ChosenChannelContext);
  const { activeUser } = useContext(AuthContext);

  useEffect(() => {
    if (activeUser && chosenChannel.id && !activeUser.channelsToAdmin.includes(chosenChannel.id)) {
      replace("/");
    }
  }, [activeUser, chosenChannel.id]);

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
