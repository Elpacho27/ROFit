import React from "react";

type LabelProps = {
  htmlFor: string;
  children: React.ReactNode;
  style?: React.CSSProperties;
};

export const Label: React.FC<LabelProps> = ({ htmlFor, children, style }) => {
  return (
    <label
      htmlFor={htmlFor}
      style={{ display: "block", marginBottom: 4, ...style }}
    >
      {children}
    </label>
  );
};
