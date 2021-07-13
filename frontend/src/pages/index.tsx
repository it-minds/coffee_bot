import ChannelList from "components/Channels/ChannelList";
import AppContainer from "components/Common/AppContainer";
import { ChannelContext } from "contexts/ChannelContext";
import { useChannelContext } from "hooks/useChannelContext";
import { NextPage } from "next";
import React from "react";

const IndexPage: NextPage = () => {
  const channelContext = useChannelContext();

  // return <Demo />;
  return (
    <AppContainer>
      <ChannelContext.Provider value={channelContext}>
        <ChannelList />
      </ChannelContext.Provider>
    </AppContainer>
  );
};

export default IndexPage;
