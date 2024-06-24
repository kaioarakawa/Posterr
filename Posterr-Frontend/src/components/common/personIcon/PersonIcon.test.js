import { render, screen, fireEvent } from "@testing-library/react";
import PersonIcon from "./PersonIcon";

const BORDER_TYPES = {
  rounded: "rounded-full",
  square: "rounded-md",
};

const SIZES = {
  sm: "h-8",
  md: "h-[64px]",
  xl: "h-32",
};

test("should render person icon", () => {
  render(<PersonIcon />);

  const element = screen.getByTestId("person-icon");

  expect(element).toBeInTheDocument();
});

test("should render person icon with different size", () => {
  render(<PersonIcon size="xl" />);

  const element = screen.getByTestId("person-icon");

  expect(element).toHaveClass(SIZES.xl);
});

test("should render person icon with different border", () => {
  render(<PersonIcon border="square" />);

  const element = screen.getByTestId("person-icon");

  expect(element).toHaveClass(BORDER_TYPES.square);
});
