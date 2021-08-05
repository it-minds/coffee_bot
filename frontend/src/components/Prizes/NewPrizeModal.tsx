import {
  Button,
  Modal,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalFooter,
  ModalHeader,
  ModalOverlay,
  useDisclosure
} from "@chakra-ui/react";
import { FcPlus } from "@react-icons/all-files/Fc/FcPlus";
import CupImage from "components/Images/CupImage";
import React, { FC } from "react";

import NewPrizeForm from "./NewPrizeForm";

const NewPrizeModal: FC = () => {
  const { isOpen, onOpen, onClose } = useDisclosure();
  return (
    <>
      <Button onClick={onOpen} leftIcon={<FcPlus size={21} />} colorScheme="blue">
        Add new prize
      </Button>

      <Modal isOpen={isOpen} onClose={onClose}>
        <ModalOverlay />
        <ModalContent>
          <ModalHeader>New Prize Details</ModalHeader>
          <ModalCloseButton />
          <ModalBody>
            <CupImage w={40} m="auto" />
            <NewPrizeForm onSuccess={onClose} />
          </ModalBody>

          <ModalFooter>
            {/* <Button colorScheme="blue" mr={3} onClick={onClose}>
              Close
            </Button>
            <Button variant="ghost">Secondary Action</Button> */}
          </ModalFooter>
        </ModalContent>
      </Modal>
    </>
  );
};

export default NewPrizeModal;
