import {
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
import PopoverMenuButton from "components/Common/PopoverMenuButton";
import { ChannelContext } from "contexts/ChannelContext";
import { FC, useCallback, useContext } from "react";
import {
  ChannelSettingsDto,
  IChannelSettingsDto,
  IChannelSettingsIdDto
} from "services/backend/nswagts";

type Props = {
  channel: IChannelSettingsIdDto;
};

const EditChannelSettingsTriggerBtn: FC<Props> = ({ channel }) => {
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
  if (!channel) return null;
  return (
    <>
      <PopoverMenuButton btnText="Edit channel settings" onClickMethod={onOpen} />
      <Modal isOpen={isOpen} onClose={onClose} scrollBehavior="inside" size="3xl">
        <ModalOverlay />
        <ModalContent>
          <ModalHeader>Edit settings for {channel.slackChannelName}</ModalHeader>
          <ModalCloseButton />
          <Divider />
          <ModalBody>
            <ChannelSettingsForm
              MetaData={
                new ChannelSettingsDto({
                  durationInDays: channel.durationInDays,
                  weekRepeat: channel.weekRepeat,
                  startsDay: channel.startsDay,
                  individualMessage: channel.individualMessage,
                  groupSize: channel.groupSize
                })
              }
              submitCallback={submitSettings}
            />
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