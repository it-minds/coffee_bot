import { HStack, Spacer, Text } from "@chakra-ui/react";
import React, { FC } from "react";
import { ChannelMemberDTO } from "services/backend/nswagts";

import ChannelActionsMenu from "./ChannelActionsMenu";

type Props = {
  membership: ChannelMemberDTO;
};
const ChannelListItem: FC<Props> = ({ membership }) => {
  return (
    <HStack>
      <Text>#{membership.channelSettings.slackChannelName}</Text>
      <Spacer />
      <ChannelActionsMenu channelId={membership.channelSettingsId} />
    </HStack>
  );
};
export default ChannelListItem;
