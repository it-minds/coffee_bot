import "ts-array-ext/groupBy";

import { Container, Flex, Heading, Image, Skeleton } from "@chakra-ui/react";
import ImageCover from "components/ImageCover/ImageCover";
import { AuthContext } from "contexts/AuthContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { NextPage } from "next";
import { useRouter } from "next/dist/client/router";
import React, { Fragment, useContext, useReducer, useState } from "react";
import ListReducer, { ListReducerActionType } from "react-list-reducer";
import { genGalleryClient } from "services/backend/apiClients";
import { StandardGroupDto } from "services/backend/nswagts";
import { ExtendedImageDto } from "types/ExtendedImageDto";
import isomorphicEnvSettings from "utils/envSettings";

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

      const envSettings = isomorphicEnvSettings();

      allImages
        .filter(x => x.hasPhoto)
        .forEach((image: ExtendedImageDto) => {
          image.publicSrc = envSettings.backendUrl + "/images/coffeegroups/" + image.photoUrl;
          setImages({
            type: ListReducerActionType.Update,
            data: image
          });
        });
    }
  }, [activeUser, query]);

  // return <Demo />;
  return (
    <Container maxW="7xl">
      <Heading textAlign="center">Gallery</Heading>
      {activeImage !== null && (
        <ImageCover image={activeImage} onClose={() => setActiveImage(null)} />
      )}
      {Object.entries(images.groupBy(x => x.finishedAt?.getFullYear() ?? 1970)).map(
        ([year, images]) => (
          <Fragment key={year}>
            <Heading size="md" textAlign="center">
              -{year}-
            </Heading>
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
          </Fragment>
        )
      )}
    </Container>
  );
};

export default IndexPage;
