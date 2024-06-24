import { fn } from "@storybook/test";
import PersonIcon from "./PersonIcon";

// More on how to set up stories at: https://storybook.js.org/docs/writing-stories#default-export
export default {
  title: "Components/PersonIcon",
  component: PersonIcon,
  parameters: {
    // Optional parameter to center the component in the Canvas. More info: https://storybook.js.org/docs/configure/story-layout
    layout: "centered",
  },
  // This component will have an automatically generated Autodocs entry: https://storybook.js.org/docs/writing-docs/autodocs
  tags: ["autodocs"],
  // More on argTypes: https://storybook.js.org/docs/api/argtypes
  argTypes: {
    border: { control: "text" },
    size: { control: "text" },
  },
  // Use `fn` to spy on the onClick arg, which will appear in the actions panel once invoked: https://storybook.js.org/docs/essentials/actions#action-args
  args: {},
};

// More on writing stories with args: https://storybook.js.org/docs/writing-stories/args
export const Simple = {
  args: {},
};

export const DifferentSize = {
  args: {
    size: "sm",
  },
};

export const DifferentBorder = {
  args: {
    border: "square",
  },
};
