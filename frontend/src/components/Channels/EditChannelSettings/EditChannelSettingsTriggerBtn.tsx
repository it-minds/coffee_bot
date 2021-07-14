import {
  Box,
  Button,
  Divider,
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
import { ChannelContext } from "contexts/ChannelContext";
import { FC, useCallback, useContext } from "react";
import { IChannelSettingsDto, IChannelSettingsIdDto } from "services/backend/nswagts";

type Props = {
  channel: IChannelSettingsIdDto;
};

const EditChannelSettingsTriggerBtn: FC<Props> = ({ channel, children }) => {
  const { onClose, onOpen, isOpen } = useDisclosure();
  const { updateChannelSettings } = useContext(ChannelContext);
  const toast = useToast();

  const submitSettings = useCallback(
    async (settings: IChannelSettingsDto) => {
      await updateChannelSettings(channel.id, settings);
      toast({
        description: "Channel Settings updated",
        status: "success",
        duration: 5000,
        isClosable: true
      });
      onClose();
    },

    [updateChannelSettings]
  );

  return (
    <>
      <Box onClick={onOpen}>{children}</Box>
      <Modal isOpen={isOpen} onClose={onClose} scrollBehavior="inside" size="3xl">
        <ModalOverlay />
        <ModalContent>
          <ModalHeader>Edit settings for {channel.slackChannelName}</ModalHeader>
          <ModalCloseButton />
          <Divider />
          <ModalBody>
            <ChannelSettingsForm channel={channel} submitCallback={submitSettings} />
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
