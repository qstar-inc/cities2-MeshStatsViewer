import { SelectedInfoSectionBase } from "cs2/bindings";
import { MeshStats } from "types/MeshStats";
import mod from "mod.json";
import * as l10n from "cs2/l10n";
import {
  FindTranslation,
  formatNumber,
  initTranslationService,
} from "shared/lang";
import { Icon, PanelFoldout, PanelSectionRow, Tooltip } from "cs2/ui";
import { GetSectionOpen, SetSectionOpen } from "shared/section";
import { FOCUS_AUTO } from "cs2/input";

import { Divider } from "shared/vanilla";

import sipSectionStyle from "./SIPSection.module.scss";
import commonStyle from "styles/common.module.scss";
import { warningIcon } from "shared/icons";
import { infoRowModule } from "shared/style";
import { SIP_MSV_NAME } from "index";

interface SIP_MSV extends SelectedInfoSectionBase {
  prefabName: string;
  hasMesh: boolean;
  meshStatsInfo: MeshStats[];
  detailed: boolean;
  lod1thres: number;
  lod2thres: number;
  tristhres: number;
  volumethres: number;
}

let meshStatsLocal: MeshStats[] = [];

export const SIP_MSV = (componentList: any): any => {
  componentList[SIP_MSV_NAME] = (props: SIP_MSV) => {
    const { translate } = l10n.useLocalization();
    initTranslationService(translate);

    if (props.hasMesh) {
      meshStatsLocal = props.meshStatsInfo;
    }
    const modNameText = mod.name;

    const SurfaceAreaText: string = FindTranslation("SurfaceAreaText");
    const VertexCountText: string = FindTranslation("VertexCountText");
    const TrisCountText: string = FindTranslation("TrisCountText");
    const TrisDensityText: string = FindTranslation("TrisDensityText");
    const TrisReductionTextText: string = FindTranslation("TrisReductionText");
    const VolumeText: string = FindTranslation("VolumeText");

    const SurfaceAreaTooltip: string = FindTranslation("SurfaceAreaTooltip");
    const VertexCountTooltip: string = FindTranslation("VertexCountTooltip");
    const TrisCountTooltip: string = FindTranslation("TrisCountTooltip");
    const TrisDensityTooltip: string = FindTranslation("TrisDensityTooltip");
    const TrisReductionTooltip: string = FindTranslation(
      "TrisReductionTooltip",
    );
    const VolumeTooltip: string = FindTranslation("VolumeTooltip");

    const NoLOD: string = FindTranslation("NoLOD");
    const NoLODButSmall: string = FindTranslation("NoLODButSmall");
    const MinimalLODReduction: string = FindTranslation("MinimalLODReduction");
    const HighTrisDensity: string = FindTranslation("HighTrisDensity");
    const ComplexSmallMesh: string = FindTranslation("ComplexSmallMesh");

    return (
      <PanelFoldout
        key={`msv-main`}
        initialExpanded={GetSectionOpen("msv-main", true)}
        onToggleExpanded={v => SetSectionOpen("msv-main", v)}
        expandFromContent={true}
        focusKey={FOCUS_AUTO}
        header={
          <PanelSectionRow
            uppercase={true}
            disableFocus={true}
            left={modNameText}
          />
        }
      >
        <PanelSectionRow
          disableFocus={true}
          left={"Prefab"}
          right={props.prefabName}
        />
        {meshStatsLocal.map((stat, i1) => {
          let prev = 0;
          let lodnum = 0;

          const noLOD: boolean =
            stat.LODCount <= 0 && stat.MeshStatData[0].TrisCount > 1000;

          const smallAsset: boolean =
            stat.MeshStatData[0].Volume <= props.volumethres;

          return (
            <>
              {i1 > 0 ? <Divider noMargin={2} /> : null}
              {meshStatsLocal.length > 1 && (
                <PanelSectionRow
                  disableFocus={true}
                  center={`Mesh #${i1 + 1} of ${meshStatsLocal.length}`}
                />
              )}
              {stat.MeshStatData.map(stat_rp => {
                const trisReductionValue: Number = formatNumber(
                  (100 * (prev - stat_rp.TrisCount)) / prev,
                );
                const trisReductionString =
                  prev == 0 ? "N/A" : `${trisReductionValue}%`;
                prev = stat_rp.TrisCount;

                const minimalLODReduction: boolean =
                  lodnum == 0 || smallAsset
                    ? false
                    : lodnum == 1
                      ? trisReductionValue < props.lod1thres
                      : trisReductionValue < props.lod2thres;

                const highTrisDensity: boolean =
                  stat_rp.TrisDensity > (props.tristhres ?? 0) / 10;

                const complexSmallMesh =
                  stat_rp.Volume < 0.05 && highTrisDensity;

                lodnum++;
                return (
                  <>
                    <PanelSectionRow
                      disableFocus={true}
                      center={stat_rp.Name}
                    />
                    <PanelSectionRow
                      disableFocus={true}
                      center={
                        <div
                          className={`${sipSectionStyle.RowFlex} ${!props.detailed && sipSectionStyle.JustifySpaceAround}`}
                        >
                          <div>
                            <Tooltip tooltip={TrisCountTooltip}>
                              <div style={{ marginBottom: "5rem" }}>
                                <span className={commonStyle.Font90}>
                                  {TrisCountText}
                                </span>
                                <br />
                                {`${formatNumber(stat_rp.TrisCount)}`}
                              </div>
                            </Tooltip>
                            {props.detailed && (
                              <Tooltip tooltip={TrisDensityTooltip}>
                                <div>
                                  <span className={commonStyle.Font90}>
                                    {TrisDensityText}
                                  </span>
                                  <br />
                                  {`${formatNumber(stat_rp.TrisDensity)}`}
                                </div>
                              </Tooltip>
                            )}
                          </div>
                          <div>
                            <Tooltip tooltip={TrisReductionTooltip}>
                              <div style={{ marginBottom: "5rem" }}>
                                <span className={commonStyle.Font90}>
                                  {minimalLODReduction && (
                                    <Tooltip tooltip={MinimalLODReduction}>
                                      <img
                                        className={infoRowModule.icon}
                                        src={warningIcon}
                                      />
                                    </Tooltip>
                                  )}
                                  {TrisReductionTextText}
                                </span>
                                <br />
                                {trisReductionString}
                              </div>
                            </Tooltip>
                            {props.detailed && (
                              <Tooltip tooltip={SurfaceAreaTooltip}>
                                <div>
                                  <span className={commonStyle.Font90}>
                                    {SurfaceAreaText}
                                  </span>
                                  <br />
                                  {`${formatNumber(stat_rp.SurfaceArea)}`}
                                </div>
                              </Tooltip>
                            )}
                          </div>
                          {props.detailed && (
                            <div>
                              <Tooltip tooltip={VertexCountTooltip}>
                                <div style={{ marginBottom: "5rem" }}>
                                  <span className={commonStyle.Font90}>
                                    {VertexCountText}
                                  </span>
                                  <br />
                                  {`${formatNumber(stat_rp.VertexCount)}`}
                                </div>
                              </Tooltip>
                              <Tooltip tooltip={VolumeTooltip}>
                                <div>
                                  <span className={commonStyle.Font90}>
                                    {VolumeText}
                                  </span>
                                  <br />
                                  {`${formatNumber(stat_rp.Volume)}`}
                                </div>
                              </Tooltip>
                            </div>
                          )}
                        </div>
                      }
                    />
                    {highTrisDensity && (
                      <PanelSectionRow
                        disableFocus={true}
                        center={
                          <>
                            <Icon
                              src={warningIcon}
                              className={`${infoRowModule.icon} ${infoRowModule.link}`}
                            />
                            {HighTrisDensity}
                          </>
                        }
                      />
                    )}
                    {complexSmallMesh && (
                      <PanelSectionRow
                        disableFocus={true}
                        center={
                          <>
                            <Icon
                              src={warningIcon}
                              className={`${infoRowModule.icon} ${infoRowModule.link}`}
                            />
                            {ComplexSmallMesh}
                          </>
                        }
                      />
                    )}
                  </>
                );
              })}
              {noLOD &&
                (smallAsset ? (
                  <PanelSectionRow disableFocus={true} center={NoLODButSmall} />
                ) : (
                  <PanelSectionRow
                    disableFocus={true}
                    center={
                      <>
                        <Icon
                          src={warningIcon}
                          className={`${infoRowModule.icon} ${infoRowModule.link}`}
                        />
                        {NoLOD}
                      </>
                    }
                  />
                ))}
            </>
          );
        })}
      </PanelFoldout>
    );
  };
  return componentList as any;
};
