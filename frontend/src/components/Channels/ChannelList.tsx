import { Box, Center, Flex, Heading, VStack } from "@chakra-ui/react";
import { ChannelContext } from "contexts/ChannelContext";
import { FC, useContext } from "react";
import { ChannelSettingsIdDto } from "services/backend/nswagts";

import ChannelListItem from "./ChannelListItem";

const ChannelList: FC = () => {
  const { channels } = useContext(ChannelContext);

  if (!channels) return null;
  return (
    <Center>
      <Flex direction="column">
        <Heading>Your Channels:</Heading>
        <Box m="5px">
          {channels.map((channel: ChannelSettingsIdDto) => (
            <ChannelListItem key={channel.id} channel={channel} />
          ))}
          {channels.map((channel: ChannelSettingsIdDto) => (
            <ChannelListItem key={channel.id} channel={channel} />
          ))}
          {channels.map((channel: ChannelSettingsIdDto) => (
            <ChannelListItem key={channel.id} channel={channel} />
          ))}
          {channels.map((channel: ChannelSettingsIdDto) => (
            <ChannelListItem key={channel.id} channel={channel} />
          ))}
        </Box>
      </Flex>
    </Center>
  );
};

export default ChannelList;
