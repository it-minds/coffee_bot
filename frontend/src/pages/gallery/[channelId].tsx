import { Container, Flex, Heading, Image } from "@chakra-ui/react";
import ImageCover from "components/ImageCover/ImageCover";
import { AuthContext } from "contexts/AuthContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { NextPage } from "next";
import { useRouter } from "next/dist/client/router";
import React, { useContext, useState } from "react";
import { genGalleryClient } from "services/backend/apiClients";
import { StandardGroupDto } from "services/backend/nswagts";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);
  const { query } = useRouter();

  const [activeImage, setActiveImage] = useState<StandardGroupDto>(null);
  const [images, setImages] = useState<StandardGroupDto[]>([]);

  useEffectAsync(async () => {
    if (activeUser && query.channelId) {
      const channelId = parseInt(query.channelId as string);
      const client = await genGalleryClient();
      const allImages: StandardGroupDto[] = await client.getAll(channelId).catch(() => []);

      const result = await Promise.all(
        allImages.map(image =>
          fetch("/api/slackBlob?photoUrl=" + image.photoUrl + "&token=" + activeUser.slackToken)
            .then(res => res.blob())
            .then(blob => {
              const urlCreator = window.URL || window.webkitURL;
              const src = urlCreator.createObjectURL(blob);

              image.photoUrl = src;
              return image;
            })
        )
      );

      setImages(result);
    }
  }, [activeUser, query]);

  // return <Demo />;
  return (
    <Container maxW="7xl">
      <Heading textAlign="center">Gallery</Heading>
      {activeImage !== null && (
        <ImageCover image={activeImage} onClose={() => setActiveImage(null)} />
      )}
      <Flex pt="4" wrap="wrap" justifyContent="center">
        {images.map(image => (
          <Image
            _hover={{
              cursor: "pointer",
              borderSize: "2px"
            }}
            maxH="300px"
            key={image.id}
            src={image.photoUrl}
            onClick={() => setActiveImage(image)}
          />
        ))}
      </Flex>
    </Container>
  );
};

export default IndexPage;
