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
import { useNSwagClient } from "hooks/useNSwagClient";
import { useRouter } from "next/router";
import React, { FC, useMemo } from "react";
import { useState } from "react";
import { useCallback } from "react";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { CreateChannelPrizeCommand, IPrizeDTO, PrizesClient } from "services/backend/nswagts";

const customDropdown = (
  setValue: (str: keyof IPrizeDTO, value: unknown) => unknown,
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
  const { query } = useRouter();
  const channelId = useMemo(() => {
    if (!query.channelId) return;
    const channelId = parseInt(query.channelId as string);
    return channelId;
  }, [query]);

  const {
    register,
    handleSubmit,
    formState: { errors },
    setValue
  } = useForm<IPrizeDTO>();

  const [loading, setLoading] = useState(false);

  const { genClient } = useNSwagClient(PrizesClient);

  const onSubmit = useCallback(
    async (data: IPrizeDTO) => {
      setLoading(true);
      data.channelSettingsId = channelId;

      const client = await genClient();
      await client.createChannelPrize(
        new CreateChannelPrizeCommand({
          input: data
        })
      );

      onSuccess();
      setLoading(false);
    },
    [channelId]
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
        <NumberInput max={1000} min={10}>
          <NumberInputField
            {...register("pointCost", { valueAsNumber: true, required: true, min: 0, max: 1000 })}
          />
          <NumberInputStepper>
            <NumberIncrementStepper />
            <NumberDecrementStepper />
          </NumberInputStepper>
        </NumberInput>
      </FormControl>

      <Button type="submit" colorScheme="green" isLoading={loading} mt={[4, 6]}>
        Create new Prize
      </Button>
    </form>
  );
};

export default NewPrizeForm;