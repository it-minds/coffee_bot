import {
  Button,
  Flex,
  IconButton,
  Popover,
  PopoverArrow,
  PopoverBody,
  PopoverCloseButton,
  PopoverContent,
  PopoverHeader,
  PopoverTrigger,
  Spacer,
  Text
} from "@chakra-ui/react";
import { BsThreeDots } from "@react-icons/all-files/bs/BsThreeDots";
import PopoverMenuButton from "components/Common/PopoverMenuButton";
import React, { FC } from "react";
import { IChannelSettingsIdDto } from "services/backend/nswagts";

type Props = {
  channel: IChannelSettingsIdDto;
};
const ChannelListItem: FC<Props> = ({ channel }) => {
  if (!channel) return null;
  return (
    <Flex rounded="lg" w="m" h="40px" justify="center" align="center" borderWidth={2} m="10px">
      <Text>{channel.slackChannelName}</Text>
      <Spacer />
      <Popover>
        <PopoverTrigger>
          <IconButton icon={<BsThreeDots />} aria-label="menu"></IconButton>
        </PopoverTrigger>
        <PopoverContent>
          <PopoverArrow />
          <PopoverBody>Are you sure you want to have that milkshake?</PopoverBody>
          <PopoverMenuButton btnText="hell" onClickMethod={() => null} />
        </PopoverContent>
      </Popover>
    </Flex>
  );
};
export default ChannelListItem;
