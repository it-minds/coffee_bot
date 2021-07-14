import { useBreadcrumbs } from "components/Breadcrumbs/useBreadcrumbs";
import ChannelList from "components/Channels/ChannelList";
import { ChannelContext } from "contexts/ChannelContext";
import { useChannelContext } from "hooks/useChannelContext";
import { NextPage } from "next";
import React from "react";

const IndexPage: NextPage = () => {
  const channelContext = useChannelContext();

  useBreadcrumbs([
    {
      name: "home",
      path: "/"
    }
  ]);

  return (
    <ChannelContext.Provider value={channelContext}>
      <ChannelList />
    </ChannelContext.Provider>
  );
};

export default IndexPage;
