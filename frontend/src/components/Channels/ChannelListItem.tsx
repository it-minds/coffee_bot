import { HStack, Spacer, Switch, Text } from "@chakra-ui/react";
import { ChannelContext } from "contexts/ChannelContext";
import React, { FC, useContext } from "react";
import { IChannelSettingsIdDto, UpdateChannelPauseInput } from "services/backend/nswagts";

import ChannelActionsMenu from "./ChannelActionsMenu";

type Props = {
  channel: IChannelSettingsIdDto;
};
const ChannelListItem: FC<Props> = ({ channel }) => {
  const { updateChannelPaused } = useContext(ChannelContext);

  return (
    <HStack>
      <Text>#{channel.slackChannelName}</Text>
      <Spacer />
      <Switch
        size="md"
        isChecked={!channel.paused}
        onChange={() => {
          updateChannelPaused(
            new UpdateChannelPauseInput({
              channelId: channel.id,
              paused: !channel.paused
            })
          );
        }}
      />
      <p>{channel.paused ? "on pause" : "active"}</p>
      <Spacer />
      <ChannelActionsMenu channel={channel} />
    </HStack>
  );
};
export default ChannelListItem;
