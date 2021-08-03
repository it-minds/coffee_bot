import { Heading } from "@chakra-ui/react";
import PrizeListOverview from "components/Prizes/PrizeListOverview";
import { withAuth } from "hocs/withAuth";
import { NextPage } from "next";
import React from "react";

const IndexPage: NextPage = () => {
  return (
    <>
      <Heading textAlign="center">Prizes</Heading>
      <PrizeListOverview />
    </>
  );
};

export default withAuth(IndexPage);
