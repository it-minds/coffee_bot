import { Box, HStack, Skeleton, Spacer, useColorModeValue } from "@chakra-ui/react";
import { MdShoppingCart } from "@react-icons/all-files/md/MdShoppingCart";
import { useHover } from "hooks/useHover";
import { useNSwagClient } from "hooks/useNSwagClient";
import React, { useCallback, useState } from "react";
import { ClaimPrizeForUserCommand, IPrizeIdDTO, PrizesClient } from "services/backend/nswagts";

import { PFC } from "./PrizeFunctionalComponent";

interface Props {
  prize: IPrizeIdDTO;
  isPurchasable?: boolean;
  addCallback?: (result: boolean) => void | Promise<void>;
}

const PurchaseButton: PFC<Props> = ({ prize, isPurchasable = true, addCallback = () => null }) => {
  const [hoverRef, isHovered] = useHover<HTMLDivElement>();
  const bgColor = useColorModeValue("green.200", "green.600");

  const [loading, setLoading] = useState(false);

  const { genClient } = useNSwagClient(PrizesClient);

  const claimPrize = useCallback(async () => {
    setLoading(true);
    const client = await genClient();

    const result = await client
      .claimPrizeForUser(
        new ClaimPrizeForUserCommand({
          prizeId: prize.id
        })
      )
      .catch(() => false);

    await addCallback(result);
    setLoading(false);
  }, []);
  return loading ? (
    <Skeleton bg={bgColor} height="100%" borderRadius={4} />
  ) : (
    <Box
      ref={hoverRef}
      onClick={isPurchasable ? claimPrize : () => null}
      onKeyPress={isPurchasable ? claimPrize : () => null}
      tabIndex={-1}
      role={isPurchasable && "button"}
      bg={bgColor}
      borderRadius={4}
      p={1}>
      <HStack>
        {isPurchasable && isHovered ? (
          <p>
            Click to buy prize for {prize.pointCost}
            <i>p</i>
          </p>
        ) : (
          <p>
            <b>Cost:</b> {prize.pointCost}
            <i>p</i>
          </p>
        )}
        <Spacer />
        {isPurchasable && <MdShoppingCart />}
      </HStack>
    </Box>
  );
};

PurchaseButton._type = "PriceFunctionalComponent";

export default PurchaseButton;
