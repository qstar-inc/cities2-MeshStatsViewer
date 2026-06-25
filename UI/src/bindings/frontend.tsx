import engine from "cohtml/cohtml";
import { bindLocalValue } from "cs2/api";

export const ClosePanel = () => {
  mainPanelBinding.update(false);
  engine.trigger("audio.playSound", "select-item", 1);
};

export const mainPanelBinding = bindLocalValue(true);

export const visibleBindings = [mainPanelBinding];

export enum PanelIndex {
  Main = 0,
}

export const togglePanel = (indexToToggle: number) => {
  const currentlyOpen = visibleBindings[indexToToggle].value;

  visibleBindings.forEach((binding, i) => {
    binding.update(i === indexToToggle ? !currentlyOpen : false);
  });
};
