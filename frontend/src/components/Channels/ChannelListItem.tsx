import {
  Flex,
  Icon,
  IconButton,
  Popover,
  PopoverArrow,
  PopoverBody,
  PopoverContent,
  PopoverTrigger,
  Spacer,
  Text
} from "@chakra-ui/react";
import { BsPause } from "@react-icons/all-files/bs/BsPause";
import { BsPlay } from "@react-icons/all-files/bs/BsPlay";
import { BsThreeDots } from "@react-icons/all-files/bs/BsThreeDots";
import PopoverMenuButton from "components/Common/PopoverMenuButton";
import { AuthContext } from "contexts/AuthContext";
import { ChannelContext } from "contexts/ChannelContext";
import React, { FC, useContext } from "react";
import { IChannelSettingsIdDto, UpdateChannelPauseInput } from "services/backend/nswagts";

type Props = {
  channel: IChannelSettingsIdDto;
};
const ChannelListItem: FC<Props> = ({ channel }) => {
  const { updateChannelPaused } = useContext(ChannelContext);
  const { activeUser } = useContext(AuthContext);
  if (!channel) return null;
  return (
    <Flex rounded="lg" w="m" h="40px" justify="center" align="center" borderWidth={2} m="10px">
      {channel.paused ? <Icon as={BsPause} /> : <Icon as={BsPlay} />}
      <Text>{channel.slackChannelName}</Text>
      <Spacer />

      <Popover>
        <PopoverTrigger>
          <IconButton icon={<BsThreeDots />} aria-label="menu"></IconButton>
        </PopoverTrigger>
        <PopoverContent>
          <PopoverArrow />
          <PopoverBody>
            <PopoverMenuButton
              btnText={channel.paused ? "Unpause this channel" : "Pause this channel"}
              onClickMethod={() =>
                updateChannelPaused(
                  new UpdateChannelPauseInput({
                    slackUserId: activeUser.slackUserId,
                    channelId: channel.id,
                    paused: !channel.paused
                  })
                )
              }
            />
          </PopoverBody>
        </PopoverContent>
      </Popover>
    </Flex>
  );
};
export default ChannelListItem;
