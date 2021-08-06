import { Icon, Input, InputGroup, InputLeftElement } from "@chakra-ui/react";
import { AiOutlineFileImage } from "@react-icons/all-files/ai/AiOutlineFileImage";
import React, { FC, useRef, useState } from "react";
import { useEffect } from "react";

interface Props {
  name: string;
  placeholder?: string;
  acceptedFileTypes: string;
  isRequired?: boolean;
  onFileSelect: (file: File) => void;
}

const FileUpload: FC<Props> = ({
  name,
  placeholder,
  acceptedFileTypes,
  onFileSelect,
  isRequired = false
}) => {
  const inputRef = useRef<HTMLInputElement>();
  const [value, setValue] = useState<File>(null);

  useEffect(() => {
    onFileSelect(value);
  }, [value]);

  return (
    <InputGroup>
      <InputLeftElement pointerEvents="none">
        <Icon as={AiOutlineFileImage} />
      </InputLeftElement>
      <input
        type="file"
        accept={acceptedFileTypes}
        name={name}
        ref={inputRef}
        onChange={() => setValue(inputRef.current.files[0])}
        style={{ display: "none" }}></input>
      <Input
        placeholder={placeholder || "Your file ..."}
        userSelect="none"
        cursor="pointer"
        onClick={() => inputRef.current.click()}
        // ref={ref}
        isReadOnly
        value={value?.name}
      />
    </InputGroup>
  );
};

export default FileUpload;
