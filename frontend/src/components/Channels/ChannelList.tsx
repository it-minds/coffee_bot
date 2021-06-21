import { Box, Center, Flex, Heading } from "@chakra-ui/react";
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
        <Heading size="lg">{`Channels you're a part of`}</Heading>
        <Box m="5px">
          {channels.map((channel: ChannelSettingsIdDto) => (
            <ChannelListItem key={channel.id} channel={channel} />
          ))}
        </Box>
      </Flex>
    </Center>
  );
};

export default ChannelList;
