import {
  Button,
  FormControl,
  FormErrorMessage,
  FormHelperText,
  FormLabel,
  Input,
  NumberDecrementStepper,
  NumberIncrementStepper,
  NumberInput,
  NumberInputField,
  NumberInputStepper,
  Select
} from "@chakra-ui/react";
import { ChosenChannelContext } from "components/Common/AppContainer/ChosenChannelContext";
import FileUpload from "components/Common/Input/FormInput";
import { useNSwagClient } from "hooks/useNSwagClient";
import React, { FC, useContext } from "react";
import { useState } from "react";
import { useCallback } from "react";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { PrizeDTO, PrizesClient } from "services/backend/nswagts";

const customDropdown = (
  setValue: (str: keyof PrizeDTO, value: unknown) => unknown,
  value: string
) => {
  if (value === "onetime") {
    setValue("isMilestone", false);
    setValue("isRepeatable", false);
  } else if (value === "isRepeatable") {
    setValue("isMilestone", false);
    setValue("isRepeatable", true);
  } else if (value === "isMilestone") {
    setValue("isMilestone", true);
    setValue("isRepeatable", false);
  }
};

interface Props {
  onSuccess: () => void | Promise<void>;
}

const NewPrizeForm: FC<Props> = ({ onSuccess }) => {
  const { chosenChannel } = useContext(ChosenChannelContext);

  const {
    register,
    handleSubmit,
    formState: { errors },
    setValue
  } = useForm<PrizeDTO>();

  const [image, setImage] = useState<File>(null);

  const [loading, setLoading] = useState(false);

  const { genClient } = useNSwagClient(PrizesClient);

  const onSubmit = useCallback(
    async (data: PrizeDTO) => {
      setLoading(true);
      data.channelSettingsId = chosenChannel.id;

      const client = await genClient();
      const prizeId = await client.createChannelPrize({
        input: data
      });

      await client.setImageForChannelPrize(
        {
          data: image,
          fileName: image.name
        },
        prizeId
      );

      onSuccess();
      setLoading(false);
    },
    [chosenChannel.id, image]
  );

  useEffect(() => {
    register("isMilestone");
    register("isRepeatable");
  }, []);

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <FormControl id="title" isInvalid={!!errors.title} isRequired>
        <FormLabel>Title</FormLabel>
        <FormHelperText>Choose a title that is concise and unique.</FormHelperText>
        <Input {...register("title", { required: true, minLength: 3, maxLength: 30 })} />
        <FormErrorMessage>Titles length must be between 3 and 30 characters.</FormErrorMessage>
      </FormControl>

      <FormControl id="description" isInvalid={!!errors.description}>
        <FormLabel>Description</FormLabel>
        <FormHelperText>Describe the prize in details.</FormHelperText>
        <Input {...register("description", { required: false, minLength: 0, maxLength: 3000 })} />
        <FormErrorMessage>Titles length must be less than 3000 characters.</FormErrorMessage>
      </FormControl>

      <FormControl id="type" isInvalid={!!errors.isMilestone} isRequired>
        <FormLabel>Type</FormLabel>
        <Select
          placeholder="Select Prize Type"
          onChange={e => customDropdown(setValue, e.target.value)}>
          <option value="onetime">One Time</option>
          <option value="isRepeatable">Repeatable</option>
          <option value="isMilestone">Milestone</option>
        </Select>
        <FormErrorMessage>Please select a matching type.</FormErrorMessage>
      </FormControl>

      <FormControl id="pointCost">
        <FormLabel>Points</FormLabel>
        <NumberInput max={1000} min={0}>
          <NumberInputField
            {...register("pointCost", { valueAsNumber: true, required: true, min: 0, max: 1000 })}
          />
          <NumberInputStepper>
            <NumberIncrementStepper />
            <NumberDecrementStepper />
          </NumberInputStepper>
        </NumberInput>
      </FormControl>

      <FormControl id="file">
        <FormLabel>File</FormLabel>
        <FormHelperText>It is always a good idea to show what the users can buy.</FormHelperText>
        {/* <Input type="file" /> */}
        <FileUpload name="file" acceptedFileTypes="image/png, image/jpeg" onFileSelect={setImage} />
        <FormErrorMessage>Titles length must be less than 3000 characters.</FormErrorMessage>
      </FormControl>

      <Button type="submit" colorScheme="green" isLoading={loading} mt={[4, 6]}>
        Create new Prize
      </Button>
    </form>
  );
};

export default NewPrizeForm;
