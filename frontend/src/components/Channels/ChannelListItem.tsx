import { HStack, Icon, Spacer, Text } from "@chakra-ui/react";
import { HiHashtag } from "@react-icons/all-files/hi/HiHashtag";
import React, { FC } from "react";
import { ChannelMemberDTO } from "services/backend/nswagts";

import ChannelActionsMenu from "./ChannelActionsMenu";

type Props = {
  membership: ChannelMemberDTO;
};
const ChannelListItem: FC<Props> = ({ membership }) => {
  return (
    <HStack>
      <Text>
        <Icon as={HiHashtag} color="green.500" mb={0.25} />
        {membership.channelSettings.slackChannelName}
      </Text>
      <Spacer />
      <ChannelActionsMenu channelId={membership.channelSettingsId} />
    </HStack>
  );
};
export default ChannelListItem;
