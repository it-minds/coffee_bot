import ChannelList from "components/Channels/ChannelList";
import { withAuth } from "hocs/withAuth";
import { NextPage } from "next";
import React from "react";

const IndexPage: NextPage = () => {
  return <ChannelList />;
};

export default withAuth(IndexPage);
