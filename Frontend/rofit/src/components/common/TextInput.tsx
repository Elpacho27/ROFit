import React from "react";
import "../../styles/Common/TextInput.css";

type TextInputProps = {
  id: string;
  type?: string;
  value: string;
  onChange: (e: React.ChangeEvent<HTMLInputElement>) => void;
  required?: boolean;
  placeholder?: string;
  style?: React.CSSProperties;
};

export const TextInput: React.FC<TextInputProps> = ({
  id,
  type = "text",
  value,
  onChange,
  required = false,
  placeholder,
  style,
}) => {
  return (
    <input
      id={id}
      className="text-input"
      type={type}
      value={value}
      onChange={onChange}
      required={required}
      placeholder={placeholder}
      style={{
        width: "100%",
        padding: 8,
        border: "none",
        outline: "none",
        borderRadius: 8,
        boxSizing: "border-box",
        ...style,
      }}
    />
  );
};
