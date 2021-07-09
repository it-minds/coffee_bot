import { Container, Flex, Heading, Image, Skeleton } from "@chakra-ui/react";
import ImageCover from "components/ImageCover/ImageCover";
import { AuthContext } from "contexts/AuthContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { NextPage } from "next";
import { useRouter } from "next/dist/client/router";
import React, { useContext, useReducer, useState } from "react";
import ListReducer, { ListReducerActionType } from "react-list-reducer";
import { genGalleryClient } from "services/backend/apiClients";
import { StandardGroupDto } from "services/backend/nswagts";
import { ExtendedImageDto } from "types/ExtendedImageDto";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);
  const { query } = useRouter();

  const [activeImage, setActiveImage] = useState<ExtendedImageDto>(null);
  const [images, setImages] = useReducer(ListReducer<ExtendedImageDto>("id"), []);

  useEffectAsync(async () => {
    if (activeUser && query.channelId) {
      const channelId = parseInt(query.channelId as string);
      const client = await genGalleryClient();
      const allImages: StandardGroupDto[] = await client.getAll(channelId).catch(() => []);

      setImages({
        type: ListReducerActionType.Reset,
        data: allImages as ExtendedImageDto[]
      });

      await Promise.all(
        allImages.map(image =>
          fetch("/api/slackBlob?photoUrl=" + image.photoUrl, {
            headers: {
              Authorization: "Bearer " + activeUser.slackToken
            }
          })
            .then(res => res.blob())
            .then(blob => {
              const urlCreator = window.URL || window.webkitURL;
              const src = urlCreator.createObjectURL(blob);

              console.log("image loaded, blobsrc:", src);

              (image as ExtendedImageDto).publicSrc = src;
              setImages({
                type: ListReducerActionType.Update,
                data: image as ExtendedImageDto
              });
              return image;
            })
        )
      );
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
        {images.map(image =>
          image.publicSrc ? (
            <Image
              _hover={{
                cursor: "pointer",
                borderSize: "2px"
              }}
              maxH="300px"
              key={image.id}
              src={image.publicSrc}
              onClick={() => setActiveImage(image)}
            />
          ) : (
            <Skeleton key={image.id} height="300px" width="300px" />
          )
        )}
      </Flex>
    </Container>
  );
};

export default IndexPage;
