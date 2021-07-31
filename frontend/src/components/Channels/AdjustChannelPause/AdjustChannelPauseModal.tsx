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
  ModalHeader,
  ModalOverlay,
  useDisclosure
} from "@chakra-ui/react";
import React, { FC } from "react";

import AdjustChannelPauseForm from "./AdjustChannelPauseForm";

type Props = {
  channelId: number;
  as?: ComponentWithAs<"button", MenuButtonProps>;
};

const AdjustChannelPauseModal: FC<Props> = ({ channelId, children, as = Button }) => {
  const { onClose, onOpen, isOpen } = useDisclosure();

  return (
    <>
      <Box as={as} onClick={onOpen}>
        {children}
      </Box>
      <Modal isOpen={isOpen} onClose={onClose} scrollBehavior="inside" size="3xl" useInert={false}>
        <ModalOverlay />
        <ModalContent>
          <ModalHeader>Participation Status</ModalHeader>
          <ModalCloseButton />
          <Divider />
          <ModalBody mb={4}>
            <AdjustChannelPauseForm channelId={channelId} onSuccess={onClose} />
          </ModalBody>
        </ModalContent>
      </Modal>
    </>
  );
};
export default AdjustChannelPauseModal;
