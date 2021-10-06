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
import { useNSwagClient } from "hooks/useNSwagClient";
import React, { FC, useCallback } from "react";
import { ChannelClient, ChannelSettingsDto } from "services/backend/nswagts";
import { logger } from "utils/logger";
import ChannelMessagesForm from "./ChannelMessagesForm";

type Props = {
  channelId: number;
  as?: ComponentWithAs<"button", MenuButtonProps>;
};

const EditChannelMessagesTriggerBtn: FC<Props> = ({ channelId, children, as = Button }) => {
  const { onClose, onOpen, isOpen } = useDisclosure();
  const toast = useToast();

  const { genClient } = useNSwagClient(ChannelClient);

  const submitMessages = useCallback(async (settings: ChannelSettingsDto) => {
    const client = await genClient();
    try {
      await client.updateChannelMessages(channelId, { settings });
      toast({
        description: "Channel Messages updated",
        status: "success",
        duration: 5000,
        isClosable: true
      });
    } catch (err) {
      logger.warn("ChannelClient.updateChannelMessages Error", err);
      toast({
        description: "Channel Messages Not Updated",
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
          <ModalHeader>Edit Channel Messages</ModalHeader>
          <ModalCloseButton />
          <Divider />
          <ModalBody>
            <ChannelMessagesForm channelId={channelId} submitCallback={submitMessages} />
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
export default EditChannelMessagesTriggerBtn;
