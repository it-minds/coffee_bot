import { Grid, GridItem, Image, useBreakpointValue } from "@chakra-ui/react";
import CupImage from "components/Images/CupImage";
import React, { FC } from "react";
import { PrizeIdDTO } from "services/backend/nswagts";
import isomorphicEnvSettings from "utils/envSettings";

interface Props {
  prize: PrizeIdDTO;
  isClaimed?: boolean;
}

const AvailablePrize: FC<Props> = ({ prize, isClaimed = false, children }) => {
  // if (!isPFC(children)) {
  //   console.log(children._type);
  //   throw Error("Tree structure require PFC");
  // }

  const breakLower = useBreakpointValue({
    base: true,
    md: false
  });

  return (
    <Grid templateRows="5fr 7fr" templateColumns="1fr 6fr 2fr">
      <GridItem rowSpan={breakLower ? 1 : 2}>
        {prize.imageName ? (
          <Image
            src={isomorphicEnvSettings().backendUrl + "/images/prizes/" + prize.imageName}
            maxW={20}
            minW={16}
            maxH={20}
            mx="auto"
          />
        ) : (
          <CupImage
            maxW={20}
            minW={16}
            maxH={20}
            mx="auto"
            filter={isClaimed ? "" : "grayscale(100%)"}
            opacity={0.8}
          />
        )}
      </GridItem>
      <GridItem p={1}>
        <p>
          <b>{prize.title}</b>
        </p>
      </GridItem>
      <GridItem>{children}</GridItem>
      <GridItem colSpan={breakLower ? 3 : 2} p={1}>
        <p>{prize.description}</p>
      </GridItem>
    </Grid>
  );
};

export default AvailablePrize;
