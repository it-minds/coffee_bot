import {
  Box,
  Button,
  ComponentWithAs,
  Divider,
  MenuButtonProps,
  Modal,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalFooter,
  ModalHeader,
  ModalOverlay,
  useDisclosure,
  useToast
} from "@chakra-ui/react";
import ChannelSettingsForm from "components/Channels/EditChannelSettings/ChannelSettingsForm";
import { useNSwagClient } from "hooks/useNSwagClient";
import React, { FC, useCallback } from "react";
import { ChannelClient, ChannelSettingsDto } from "services/backend/nswagts";
import { logger } from "utils/logger";

type Props = {
  channelId: number;
  as?: ComponentWithAs<"button", MenuButtonProps>;
};

const EditChannelSettingsTriggerBtn: FC<Props> = ({ channelId, children, as = Button }) => {
  const { onClose, onOpen, isOpen } = useDisclosure();
  const toast = useToast();

  const { genClient } = useNSwagClient(ChannelClient);

  const submitSettings = useCallback(async (settings: ChannelSettingsDto) => {
    const client = await genClient();
    try {
      await client.updateChannelSettings(channelId, { settings });
      toast({
        description: "Channel Settings updated",
        status: "success",
        duration: 5000,
        isClosable: true
      });
    } catch (err) {
      logger.warn("ChannelClient.updateChannelSettings Error", err);
      toast({
        description: "Channel Settings Not Updated",
        status: "error",
        duration: 5000,
        isClosable: true
      });
    }

    onClose();
  }, []);

  return (
    <>
      <Box as={as} onClick={onOpen}>
        {children}
      </Box>
      <Modal isOpen={isOpen} onClose={onClose} scrollBehavior="inside" size="3xl" useInert={false}>
        <ModalOverlay />
        <ModalContent>
          <ModalHeader>Edit Channel Settings</ModalHeader>
          <ModalCloseButton />
          <Divider />
          <ModalBody>
            <ChannelSettingsForm channelId={channelId} submitCallback={submitSettings} />
          </ModalBody>
          <Divider />
          <ModalFooter>
            <Button colorScheme="blue" mr={3} onClick={onClose}>
              Close
            </Button>
          </ModalFooter>
        </ModalContent>
      </Modal>
    </>
  );
};
export default EditChannelSettingsTriggerBtn;
