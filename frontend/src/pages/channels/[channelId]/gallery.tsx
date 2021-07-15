import "ts-array-ext/groupBy";
import "ts-array-ext/sortByAttr";

import { Flex, Heading, Image, Skeleton } from "@chakra-ui/react";
import { useBreadcrumbs } from "components/Breadcrumbs/useBreadcrumbs";
import ImageCover from "components/ImageCover/ImageCover";
import { AuthContext } from "contexts/AuthContext";
import { useEffectAsync } from "hooks/useEffectAsync";
import { useNSwagClient } from "hooks/useNswagClient";
import { NextPage } from "next";
import { useRouter } from "next/dist/client/router";
import React, { Fragment, useContext, useReducer, useState } from "react";
import ListReducer, { ListReducerActionType } from "react-list-reducer";
import { GalleryClient, StandardGroupDto } from "services/backend/nswagts";
import { ExtendedImageDto } from "types/ExtendedImageDto";
import isomorphicEnvSettings from "utils/envSettings";

const IndexPage: NextPage = () => {
  const { activeUser } = useContext(AuthContext);
  const { query } = useRouter();

  useBreadcrumbs([
    {
      name: "home",
      path: "/"
    },
    {
      name: "channel " + query.channelId,
      path: "/channels/[channelId]/rounds",
      asPath: `/channels/${query.channelId}/rounds`
    },
    {
      name: "gallery",
      path: "/channels/[channelId]/gallery",
      asPath: `/channels/${query.channelId}/gallery`
    }
  ]);

  const [activeImage, setActiveImage] = useState<ExtendedImageDto>(null);
  const [images, setImages] = useReducer(ListReducer<ExtendedImageDto>("id"), []);
  const { genClient } = useNSwagClient<GalleryClient>(GalleryClient);

  useEffectAsync(async () => {
    if (!activeUser || !query.channelId) return;

    const channelId = parseInt(query.channelId as string);

    const client = await genClient();
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
  }, [activeUser, query]);

  // return <Demo />;
  return (
    <>
      <Heading textAlign="center">Gallery</Heading>
      {activeImage !== null && (
        <ImageCover image={activeImage} onClose={() => setActiveImage(null)} />
      )}
      {Object.entries(images.groupBy(x => x.finishedAt?.getFullYear() ?? 1970))
        .sortByAttr(([year]) => year, "DESC")
        .map(([year, images]) => (
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
        ))}
    </>
  );
};

export default IndexPage;
