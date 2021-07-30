import { Button, HStack, Select } from "@chakra-ui/react";
import React, { useState } from "react";
import { FC } from "react";

const options = {
  label: "channels",
  options: [
    { value: "blue", label: "Blue", color: "#0052CC" },
    { value: "purple", label: "Purple", color: "#5243AA" },
    { value: "red", label: "Red", color: "#FF5630" },
    { value: "orange", label: "Orange", color: "#FF8B00" },
    { value: "yellow", label: "Yellow", color: "#FFC400" },
    { value: "green", label: "Green", color: "#36B37E" }
  ]
};

const NewChannelButton: FC = () => {
  const [consent, setConsent] = useState(false);

  return (
    <HStack>
      <Button hidden={consent} onClick={() => setConsent(true)}>
        Select Buddy Channel
      </Button>
      <Select
        animation="slide-in"
        w={64}
        marginInlineStart="0 !important"
        hidden={!consent}
        name="colors"
        placeholder="Select channel...">
        {options.options.map(x => (
          <option value={x.value} key={x.value}>
            {x.color}
          </option>
        ))}
      </Select>
      <Button hidden={!consent} onClick={() => setConsent(false)}>
        Create Buddy Channel
      </Button>
    </HStack>
  );
};

export default NewChannelButton;
