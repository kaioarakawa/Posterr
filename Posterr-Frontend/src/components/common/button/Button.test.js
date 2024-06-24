import { render, screen, fireEvent } from "@testing-library/react";
import Button from "./Button";

test("should render button with text when provided", () => {
  render(<Button text="Enter" />);

  const element = screen.getByTestId("button");

  expect(element).toBeInTheDocument();
});

test("should render disabled button", () => {
  render(<Button disabled />);

  const element = screen.getByTestId("button");

  expect(element).toBeDisabled();
});

test("should call action when button is trigged", () => {
  const action = jest.fn();

  render(<Button action={action} />);

  const element = screen.getByTestId("button");
  fireEvent.click(element);

  expect(action).toHaveBeenCalledTimes(1);
});
