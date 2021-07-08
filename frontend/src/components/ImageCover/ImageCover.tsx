import {
  Button,
  Image,
  Modal,
  ModalBody,
  ModalCloseButton,
  ModalContent,
  ModalFooter,
  ModalHeader,
  ModalOverlay,
  Text,
  VStack
} from "@chakra-ui/react";
import React, { FC } from "react";
import { StandardGroupDto } from "services/backend/nswagts";

type Props = {
  image: StandardGroupDto;
  onClose: () => void;
};

const ImageCover: FC<Props> = ({ image, onClose }) => {
  return (
    <Modal onClose={onClose} size="5xl" isOpen={true}>
      <ModalOverlay />
      <ModalContent>
        <ModalHeader>Image</ModalHeader>
        <ModalCloseButton />
        <ModalBody>
          <VStack>
            <Image src={image.photoUrl}></Image>
            <Text as="b">Members:</Text>
            <Text>{image.members.join(" & ")}</Text>
          </VStack>
        </ModalBody>
        <ModalFooter>
          <Button onClick={onClose}>Close</Button>
        </ModalFooter>
      </ModalContent>
    </Modal>
  );
};

export default ImageCover;
