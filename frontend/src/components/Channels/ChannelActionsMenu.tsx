import {
  Button,
  IconButton,
  Menu,
  MenuButton,
  MenuDivider,
  MenuGroup,
  MenuItem,
  MenuList,
  useBreakpointValue
} from "@chakra-ui/react";
import { BsBoxArrowInRight } from "@react-icons/all-files/bs/BsBoxArrowInRight";
import { AuthContext } from "contexts/AuthContext";
import { useRouter } from "next/router";
import React, { useContext } from "react";
import { FC } from "react";

import AdjustChannelPauseModal from "./AdjustChannelPause/AdjustChannelPauseModal";
import EditChannelSettingsTriggerBtn from "./EditChannelSettings/EditChannelSettingsTriggerBtn";

type Props = {
  channelId: number;
};
const ChannelActionsMenu: FC<Props> = ({ channelId }) => {
  const { activeUser } = useContext(AuthContext);
  const router = useRouter();

  const smallButton = useBreakpointValue({
    base: true,
    md: false
  });

  return (
    <Menu>
      <MenuButton
        as={smallButton ? IconButton : Button}
        _hover={{
          bg: "pink.500"
        }}
        _active={{
          bg: "blue.300"
        }}
        bg="blue.400"
        color="white"
        {...{
          [smallButton ? "icon" : "leftIcon"]: <BsBoxArrowInRight />
        }}
        m="auto"
        size="sm">
        {smallButton ? "" : "Navigation Menu"}
      </MenuButton>

      <MenuList maxW="90vw">
        <MenuGroup title="General">
          <MenuItem
            onClick={() =>
              router.push("/channels/[channelId]/gallery", "/channels/" + channelId + "/gallery")
            }>
            Gallery
          </MenuItem>
          <MenuItem
            onClick={() =>
              router.push("/channels/[channelId]/stats", "/channels/" + channelId + "/stats")
            }>
            Stats
          </MenuItem>
          <MenuItem
            onClick={() =>
              router.push(
                "/channels/[channelId]/rounds/active",
                "/channels/" + channelId + "/rounds/active"
              )
            }>
            Active Round
          </MenuItem>
          <MenuItem
            onClick={() =>
              router.push("/channels/[channelId]/rounds", "/channels/" + channelId + "/rounds")
            }>
            Previous Rounds
          </MenuItem>
          <MenuItem
            onClick={() =>
              router.push(
                "/channels/[channelId]/prizes/mine",
                "/channels/" + channelId + "/prizes/mine"
              )
            }>
            My Prizes
          </MenuItem>
          <AdjustChannelPauseModal channelId={channelId} as={MenuItem}>
            Edit your participation
          </AdjustChannelPauseModal>
        </MenuGroup>

        {activeUser?.channelsToAdmin.includes(channelId) && (
          <>
            <MenuDivider />
            <MenuGroup title="Admin">
              <EditChannelSettingsTriggerBtn channelId={channelId} as={MenuItem}>
                Edit Channel Settings
              </EditChannelSettingsTriggerBtn>
              <MenuItem
                onClick={() =>
                  router.push(
                    "/channels/[channelId]/admin/prizes",
                    "/channels/" + channelId + "/admin/prizes"
                  )
                }>
                Edit Channel Prizes
              </MenuItem>
              <MenuItem
                onClick={() =>
                  router.push(
                    "/channels/[channelId]/admin/prizes/claims",
                    "/channels/" + channelId + "/admin/prizes/claims"
                  )
                }>
                Handle Prize Claims
              </MenuItem>
            </MenuGroup>
          </>
        )}
      </MenuList>
    </Menu>
  );
};

export default ChannelActionsMenu;
