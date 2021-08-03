import { Heading } from "@chakra-ui/react";
import { ChosenChannelContext } from "components/Common/AppContainer/ChosenChannelContext";
import PrizeClaimsList from "components/PrizeClaims/PrizeClaimsList";
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
      <Heading textAlign="center">Prize Claims</Heading>

      <PrizeClaimsList />
    </>
  );
};

export default withAuth(IndexPage);
